defmodule UnsocialVR.FFormations do
  @moduledoc """
  Compute f-formations using the GCFF algorithm, running on a separate server.
  """

  use GenServer

  def start_link() do
    GenServer.start_link(__MODULE__, [], name: __MODULE__)
  end

  def init(state) do
    schedule_analysis()
    {:ok, state}
  end

  @analysis_period :timer.seconds(1)

  def schedule_analysis() do
    Process.send_after(__MODULE__, :analyze, @analysis_period)
  end

  def handle_info(:analyze, state) do
    UnsocialVR.Cache.get_players()
    |> analyze()
    |> Enum.map(fn {id, ff} -> UnsocialVR.Cache.put_f_formation(id, ff) end)

    schedule_analysis()
    {:noreply, state}
  end

  @gcff_server "http://127.0.0.1:5000/continuous"

  @doc """
  Analyze f-formations using the GCFF server.
  """
  def analyze([]), do: []
  def analyze(all_players) do
    all_players
    |> Stream.map(&prepare_data_for_gcff/1)
    |> Stream.map(&Enum.join(&1, ","))
    |> Enum.join("\n")
    |> gcff()
    |> Poison.decode!()
    |> Enum.map(fn map -> Map.pop(map, "id") end)
  end

  @doc """
  Simplify the data from the server to the GCFF algorithm.
  """
  def prepare_data_for_gcff(player_data) do
    position_map = Map.fetch!(player_data, "chestPosition")
    rotation_map = Map.fetch!(player_data, "chestRotation")
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

end
