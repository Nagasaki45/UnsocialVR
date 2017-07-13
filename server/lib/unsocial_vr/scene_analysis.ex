defmodule UnsocialVR.SceneAnalysis do
  @moduledoc """
  Players interact with the multiple 'social behaviour' modifications through
  this module. Specifically, it caches the player data, provide information
  about other player from the cache, which is updated with autopilots,
  f-formation analysis, and automated nodding.
  """

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
    speaker_id = Cache.get_f_formation_speaker_id(my_f_formation_id)
    my_autopilots = Cache.get_autopilots(my_f_formation_id)

    all_players
    |> Stream.filter(fn player -> player.id != local_id end)
    |> Enum.map(fn p ->
      cond do
        (p.id in my_autopilots) -> autopilot(p, speaker_id)
        !(p.id in my_f_formation["members"]) -> Map.put(p, :state, :ignored)
        true -> Map.put(p, :state, :real)
      end
    end)
  end

  @doc """
  Replace the player transforms with recorded autopilot behaviour + nodding.
  """
  def autopilot(player, nil) do
    chest_position = player["chestPosition"]
    position_shift = {chest_position["x"], chest_position["z"]}
    time_shift = player.id * 4321  # Just to cause difference between players
    autopilot_data = UnsocialVR.Autopilot.play(position_shift, time_shift)
    player
    |> Map.merge(autopilot_data)
    |> Map.put(:state, :autopilot)
  end
  def autopilot(player, speaker_id) do
    speaker = Cache.get_player(speaker_id)
    UnsocialVR.Backchannel.add_prediction_job(player, speaker)
    player
    |> Map.put(:attention, speaker_id)
    |> Map.put(:nodding, Cache.get_backchannel(player.id))
    |> autopilot(nil)
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
