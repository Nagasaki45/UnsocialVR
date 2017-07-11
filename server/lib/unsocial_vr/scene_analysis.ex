defmodule UnsocialVR.SceneAnalysis do

  alias UnsocialVR.Cache

  @doc """
  Cache the player data, and if speaking, mark as the current speaker in the
  f-formation.
  """
  def cache_player(local_id, player_data) do
    UnsocialVR.Cache.put_player(local_id, player_data)
    if player_data["isTalking"] do
      local_id
      |> Cache.get_player_f_formation_id()
      |> Cache.put_f_formation_speaker_id(local_id)
    end
  end

  @doc """
  How I see the other players.

  - Players that intentionally autopilot towards me will... autopilot.
  - Ignore players that are not in my f-formation.
  - The rest are real.
  """
  def remote_players(local_id) do
    all_players = Cache.get_players()
    my_f_formation_id = Cache.get_player_f_formation_id(local_id)
    my_f_formation = Cache.get_f_formation(my_f_formation_id)
    my_f_formation_speaker = Cache.get_f_formation_speaker(my_f_formation_id)
    my_autopilots = Cache.get_autopilots(my_f_formation_id)

    all_players
    |> Stream.filter(fn player -> player.id != local_id end)
    |> Enum.map(fn p ->
      cond do
        (p.id in my_autopilots) -> autopilot(p, my_f_formation_speaker)
        !(p.id in my_f_formation["members"]) -> Map.put(p, :state, :ignored)
        true -> Map.put(p, :state, :real)
      end
    end)
  end

  @doc """
  Replace the player transforms with recorded autopilot behaviour + nodding.
  """
  def autopilot(player, speaker) do
    UnsocialVR.Backchannel.add_prediction_job(player, speaker)
    nodding = Cache.get_backchannel(player.id)
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
  def start_autopilot(local_id) do
    my_f_formation_id = Cache.get_player_f_formation_id(local_id)
    Cache.put_autopilot(local_id, my_f_formation_id)
  end

  @doc """
  Stop faking social behaviour.
  """
  def stop_autopilot(local_id) do
    Cache.delete_autopilot(local_id)
  end
end
