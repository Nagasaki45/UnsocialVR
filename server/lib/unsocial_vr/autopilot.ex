defmodule UnsocialVR.Autopilot do
  @moduledoc """
  Players in UnsocialVR can pretend to listen to you. What you see in this
  case is a player in "autopilot" mode.
  The autopilot mode is a recorded behaviour, managed in this module.
  """

  use GenServer
  require Logger

  @data_filename "autopilot_data.tab"
  @recording_period 100  # In milliseconds

  @doc """
  Start recording a new autopilot behaviour.
  Should be called manually from iex (elixir shell).
  """
  def start_recording(player_id) do
    GenServer.cast(__MODULE__, {:start_recording, player_id})
  end

  @doc """
  Stop recording the running autopilot behaviour.
  Data will be automatically shifted to origin position.
  NOTICE: it will overide existing data.
  Should be called manually from iex (elixir shell).
  """
  def stop_recording() do
    Process.send(__MODULE__, :stop_recording, [])
  end

  @doc """
  Query autopilot data.

  - position_shift: an {x, z} tuple to shift autopilot from origin.
  - time_shift: the number of millisecond to shift the output, so not all
                players autopiloting look the same.
  """
  def play(position_shift, time_shift) do
    :milliseconds
    |> System.monotonic_time()
    |> Kernel.+(time_shift)
    |> div(@recording_period)
    |> rem(:ets.info(__MODULE__, :size))
    |> lookup()
    |> shift_character(position_shift)
  end

  @doc """
  The autopilot is a GenServer that maintains an ETS table.
  When the process terminates it writes the data to disk.
  On startup it reads the table from disk.
  """
  def start_link() do
    GenServer.start_link(__MODULE__, nil, name: __MODULE__)
  end

  # GenServer callbacks

  def init(_state) do
    PersistentEts.new(__MODULE__, @data_filename, [:named_table])
    {:ok, nil}
  end

  def handle_cast({:start_recording, player_id}, nil) do
    :ets.delete_all_objects(__MODULE__)
    recorder(player_id)  # Loops until :stop_recording
    data = :ets.tab2list(__MODULE__)
    data = finilize_recording(data)
    :ets.insert(__MODULE__, data)
    {:noreply, nil}
  end

  # Internals

  def recorder(player_id, frame \\ 0) do
    take_snapshot(player_id, frame)
    receive do
      :stop_recording -> :ok  # Just break the loop
    after
      @recording_period -> recorder(player_id, frame + 1)
    end
  end

  def take_snapshot(player_id, frame) do
    transforms_data = get_transforms_from_cache(player_id)
    :ets.insert(__MODULE__, {frame, transforms_data})
  end

  def get_transforms_from_cache(player_id) do
    :cache
    |> ConCache.get(player_id)
    |> Enum.filter(&is_transform/1)
    |> Enum.into(Map.new())
  end

  def is_transform({key, _val}) do
    cond do
      String.ends_with?(key, "Position") -> true
      String.ends_with?(key, "Rotation") -> true
      true -> false
    end
  end

  @doc """
  Lookup autopilot frame from ETS.
  """
  def lookup(frame) do
    case :ets.lookup(__MODULE__, frame) do
      [{_key, player_data}] -> player_data
      [] -> Map.new()
    end
  end

  @doc """
  Shift the character to arbitrary position by changing the chestPosition,
  the rest are relative (localPositions).
  """
  def shift_character(player_data, {x, z}) do
    player_data
    |> update_in(["chestPosition", "x"], &(&1 + x))
    |> update_in(["chestPosition", "z"], &(&1 + z))
  end

  def finilize_recording(data) do
    # Calculate mean {x, z} position to shift by
    shift = {-calculate_mean(data, "x"), -calculate_mean(data, "z")}
    Logger.info("Autopilot origin shifts by #{inspect(shift)}")
    # Shift the entire positions in the table by {-x, -z}
    Enum.map(data, fn {frame, transforms} ->
      {frame, shift_character(transforms, shift)}
    end)
  end

  def calculate_mean(data, x_or_z) do
    data
    |> Stream.map(fn {_frame, transforms} -> transforms["chestPosition"][x_or_z] end)
    |> Enum.sum()
    |> Kernel./(length(data))
  end

end
