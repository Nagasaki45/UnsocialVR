defmodule UnsocialVR.FFormations do
  @moduledoc """
  Compute f-formations using the GCFF algorithm, running on a separate server.
  """

  use GenServer

  def start_link() do
    GenServer.start_link(__MODULE__, [], name: __MODULE__)
  end

  def get() do
    GenServer.call(__MODULE__, :get)
  end

  @analysis_period :timer.seconds(1)

  def init(f_formations) do
    schedule_analysis()
    {:ok, f_formations}
  end

  def schedule_analysis() do
    Process.send_after(__MODULE__, :analyze, @analysis_period)
  end

  def handle_call(:get, _from, f_formations) do
    {:reply, f_formations, f_formations}
  end

  def handle_info(:analyze, _f_formations) do
    f_formations = analyze()
    schedule_analysis()
    {:noreply, f_formations}
  end

  @gcff_server "http://127.0.0.1:5000"

  @doc """
  Using data from the cache analyze f-formations on GCFF server.
  """
  def analyze() do
    all_players = UnsocialVR.Cache.get_players()
    all_players
    |> Stream.map(&prepare_data_for_gcff/1)
    |> Stream.map(&Enum.join(&1, ","))
    |> Enum.join("\n")
    |> gcff()
    |> String.split(",")
    |> Enum.map(&String.to_integer/1)
    |> group_by_ids(all_players)
    |> IO.inspect()
  end

  @doc """
  Simplify the data from the server to the GCFF algorithm.
  """
  def prepare_data_for_gcff(player_data) do
    # TODO change to chest
    position_map = Map.fetch!(player_data, "headPosition")
    rotation_map = Map.fetch!(player_data, "headRotation")
    [
      Map.fetch!(player_data, :id),
      Map.fetch!(position_map, "x"),
      Map.fetch!(position_map, "z"),
      yaw(rotation_map),
    ]
  end

  @doc """
  Compute yaw (y-axis angle, we are speaking with unity, in radians)
  from quaternion. Correct the value to what GCFF expect.
  """
  def yaw(rotation_map) do
    w = Map.fetch!(rotation_map, "w")
    x = Map.fetch!(rotation_map, "x")
    y = Map.fetch!(rotation_map, "y")
    z = Map.fetch!(rotation_map, "z")
    yaw = :math.atan2(2 * y * w - 2 * x * z, 1 - 2 * y * y - 2 * z * z)
    # Rotate to correct 0 angle
    yaw = -yaw + :math.pi / 2
    # Unwrap
    if yaw < 0 do
      yaw + 2 * :math.pi
    else
      yaw
    end
  end

  @doc """
  Call the GCFF server.
  """
  def gcff(features) do
    opts = [body: features, headers: ["Content-Type": "text/csv"]]
    %{status_code: 200, body: body} = HTTPotion.post(@gcff_server, opts)
    body
  end

  def group_by_ids(gcff_output, all_players) do
    all_players
    |> Stream.map(fn player_data -> player_data.id end)
    |> group_by_key(gcff_output)
  end

  def group_by_key(items, keys) do
    keys
    |> Enum.zip(items)
    |> Enum.group_by(fn {key, _item} -> key end)
    |> Enum.map(fn {_key, keys_and_items} ->
      keys_and_items |> Enum.map(fn {_key, item} -> item end)
    end)
  end

end
