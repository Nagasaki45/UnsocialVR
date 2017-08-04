defmodule UnsocialVR.SceneAnalysis do
  @moduledoc """
  Players interact with the multiple 'social behaviour' modifications through
  this module. Specifically, it caches the player data, provide information
  about other player from the cache, which is updated with autopilots,
  and automated nodding.
  """

  alias UnsocialVR.Autopilot
  alias UnsocialVR.Backchannel
  alias UnsocialVR.Cache

  @doc """
  Cache the player data.
  Mark as the current speaker if speaking.
  """
  def cache_player(local_id, player_data) do
    Cache.put_player(local_id, player_data)
    transforms = get_transforms(player_data)
    Autopilot.record(local_id, transforms)
    if player_data["isTalking"] do
      Cache.put_speaker_id(local_id)
    end
  end

  @doc """
  How I see the other players.

  - Players that intentionally autopilot will... autopilot.
  - The rest are real.
  """
  def remote_players(local_id) do
    all_players = Cache.get_players()
    speaker_id = Cache.get_speaker_id()
    autopilots = Cache.get_autopilots()

    all_players
    |> Stream.filter(fn player -> player.id != local_id end)
    |> Enum.map(fn p ->
      if p.id in autopilots do
        autopilot(p, speaker_id)
      else
        Map.put(p, :state, :real)
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
  Start faking social behaviour.
  """
  def start_autopilot(local_id) do
    Cache.put_autopilot(local_id)
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
