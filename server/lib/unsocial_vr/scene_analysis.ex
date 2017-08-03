defmodule UnsocialVR.SceneAnalysis do
  @moduledoc """
  Players interact with the multiple 'social behaviour' modifications through
  this module. Specifically, it caches the player data, provide information
  about other player from the cache, which is updated with autopilots,
  f-formation analysis, and automated nodding.
  """

  alias UnsocialVR.Autopilot
  alias UnsocialVR.Backchannel
  alias UnsocialVR.Cache
  alias UnsocialVR.FFormations

  @doc """
  Cache the player data.
  If it's a new player force an f-formation analysis (blocking).
  If speaking, mark as the current speaker in the f-formation.
  """
  def cache_player(local_id, player_data) do
    case Cache.update_existing_player(local_id, player_data) do
      {:error, :not_existing} ->
        Cache.put_player(local_id, player_data)
        FFormations.force_analysis()
      :ok -> :ok
    end
    transforms = get_transforms(player_data)
    Autopilot.record(local_id, transforms)
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
    autopilot_data = Autopilot.play(player.id)
    player
    |> Map.merge(autopilot_data)
    |> Map.put(:state, :autopilot)
  end
  def autopilot(player, speaker_id) do
    speaker = Cache.get_player(speaker_id)
    Backchannel.add_prediction_job(player, speaker)
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
    Autopilot.set_recording(local_id, false)
  end

  @doc """
  Stop faking social behaviour.
  """
  def stop_autopilot(local_id) do
    Cache.delete_autopilot(local_id)
    Autopilot.set_recording(local_id, true)
  end

  defp get_transforms(player_data) do
    player_data
    |> Stream.filter(&transform?/1)
    |> Enum.into(Map.new())
  end

  defp transform?({key, _val}) do
    cond do
      String.ends_with?(key, "Position") -> true
      String.ends_with?(key, "Rotation") -> true
      true -> false
    end
  end
end
