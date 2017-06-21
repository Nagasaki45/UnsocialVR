defmodule UnsocialVR.SceneAnalysis do

  @doc """
  How I see the other players.

  - Players that intentionally autopilot towards me will... autopilot.
  - Ignore players that are not in my f-formation.
  - The rest are real.
  """
  def remote_players(local_id) do
    all_players = UnsocialVR.Cache.get_players()
    my_f_formation_id = UnsocialVR.Cache.get_player_f_formation_id(local_id)
    my_f_formation = UnsocialVR.Cache.get_f_formation(my_f_formation_id)
    my_autopilots = UnsocialVR.Cache.get_autopilots(my_f_formation_id)

    all_players
    |> Stream.filter(fn player -> player.id != local_id end)
    |> Enum.map(fn p ->
      cond do
        (p.id in my_autopilots) -> autopilot(p)
        !(p.id in my_f_formation["members"]) -> Map.put(p, :state, :ignored)
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
    my_f_formation_id = UnsocialVR.Cache.get_player_f_formation_id(local_id)
    UnsocialVR.Cache.add_autopilot(my_f_formation_id, local_id)
  end

  @doc """
  When stopped, drop myself from the autopilots of this f-formation.
  """
  def stop_autopiloting(local_id, f_formation_id) do
    UnsocialVR.Cache.remove_autopilot(f_formation_id, local_id)
  end
end
