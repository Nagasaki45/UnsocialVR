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

  def put_player(player_id, data) do
    ConCache.put(cache(), {:player_data, player_id}, data)
  end

  def get_f_formation(player_id) do
    cache()
    |> ConCache.get(:f_formations)
    |> Enum.find([], fn ff -> player_id in ff end)
  end

  def put_f_formations(f_formations) do
    ConCache.put(cache(), :f_formations, f_formations)
  end

  def get_autopilots(id) do
    autopilots = get_autopilots_from_ets(id)
    # We must manually touch the autopilot data to keep it alive.
    # There are no enough writes, and the reads bypass ConCache.
    Enum.each(autopilots, &ConCache.touch(cache(), {:autopilot, &1, id}))
    autopilots
  end

  def get_autopilots_from_ets(id) do
    cache()
    |> ConCache.ets()
    |> :ets.match({{:autopilot, :"$1", id}, 1})
    |> Enum.map(fn [id] -> id end)
  end

  def add_autopilots(faker_id, targets_ids) do
    Enum.each(targets_ids, fn target_id ->
      ConCache.put(cache(), {:autopilot, faker_id, target_id}, 1)
    end)
  end

  def remove_autopilots(faker_id, targets_ids) do
    Enum.each(targets_ids, fn target_id ->
      ConCache.delete(cache(), {:autopilot, faker_id, target_id})
    end)
  end
end
