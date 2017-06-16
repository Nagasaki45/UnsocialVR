defmodule UnsocialVR.SceneAnalysis do

  @doc """
  How I see the other players.

  - Players that intentionally autopilot towards me will... autopilot.
  - Ignore players that are not in my f-formation.
  - The rest are real.
  """
  def remote_players(local_id) do
    all_players = UnsocialVR.Cache.get_players()
    my_autopilots = UnsocialVR.Cache.get_autopilots(local_id)
    my_f_formation = UnsocialVR.Cache.get_f_formation(local_id)

    all_players
    |> Stream.filter(fn player -> player.id != local_id end)
    |> Enum.map(fn p ->
      cond do
        (p.id in my_autopilots) -> autopilot(p)
        !(p.id in my_f_formation) -> Map.put(p, :state, :ignored)
        true -> Map.put(p, :state, :real)
      end
    end)
  end

  @doc """
  Replace the player transforms with recorded autopilot behaviour.
  """
  def autopilot(player) do
    chest_position = player["chestPosition"]
    position_shift = {chest_position["x"], chest_position["z"]}
    time_shift = player.id * 4321  # Just to cause difference between players
    autopilot_data = UnsocialVR.Autopilot.play(position_shift, time_shift)
    player
    |> Map.merge(autopilot_data)
    |> Map.put(:state, :autopilot)
  end

  @doc """
  Player start autopilot behaviour in the current f-formation.
  """
  def start_autopiloting(local_id) do
    my_f_formation =
      local_id
      |> UnsocialVR.Cache.get_f_formation()
      |> Enum.filter(fn id -> id != local_id end)
    UnsocialVR.Cache.add_autopilots(local_id, my_f_formation)
  end

  @doc """
  When stopped, there is a reference player that we stopped faking behaviour
  to. We will also stop faking towards the entire f-formation of this player.
  """
  def stop_autopiloting(local_id, reference_player_id) do
    f_formation = UnsocialVR.Cache.get_f_formation(reference_player_id)
    UnsocialVR.Cache.remove_autopilots(local_id, f_formation)
  end
end
