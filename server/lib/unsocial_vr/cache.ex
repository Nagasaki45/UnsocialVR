defmodule UnsocialVR.Cache do
  @moduledoc """
  A single place to organize all cache operations.
  """

  def cache() do
    :cache
  end

  def get_players() do
    cache()
    |> ConCache.ets()
    |> :ets.match({{:player_data, :"$1"}, :"$2"})
    |> Enum.map(fn [id, player_data] -> Map.put(player_data, :id, id) end)
  end

  def get_player(player_id) do
    ConCache.get(cache(), {:player_data, player_id})
  end

  def put_player(player_id, data) do
    ConCache.put(cache(), {:player_data, player_id}, data)
  end

  def get_speaker_id() do
    ConCache.get(cache(), :speaker)
  end

  def put_speaker_id(player_id) do
    ConCache.put(cache(), :speaker, player_id)
  end

  def get_autopilots() do
    cache()
    |> ConCache.ets()
    |> :ets.match({{:autopilot, :"$1"}, true})
    |> Enum.map(fn [player_id] -> player_id end)
    |> _touch()  # Necessary for keeping the data in cache
  end

  def put_autopilot(player_id) do
    ConCache.put(cache(), {:autopilot, player_id}, true)
  end

  def delete_autopilot(player_id) do
    ConCache.delete(cache(), {:autopilot, player_id})
  end

  def get_backchannel(autopilot_id) do
    ConCache.get(cache(), {:backchannel, autopilot_id})
  end

  def put_backchannel(autopilot_id, prediction) do
    ConCache.put(cache(), {:backchannel, autopilot_id}, prediction)
  end

  def _touch(player_ids) do
    Enum.each(player_ids, fn player_id ->
      ConCache.touch(cache(), {:autopilot, player_id})
    end)

    player_ids
  end
end
