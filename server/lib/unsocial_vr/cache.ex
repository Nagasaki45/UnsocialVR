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

  def get_player_f_formation_id(player_id) do
    ConCache.get(cache(), {:player_f_formation_id, player_id})
  end

  def get_f_formation(id) do
    ConCache.get(cache(), {:f_formation, id})
  end

  def put_f_formation(id, f_formation) do
    ConCache.put(cache(), {:f_formation, id}, f_formation)
    # Set the f-formation of each player
    Enum.each(f_formation["members"], fn player_id ->
      ConCache.put(cache(), {:player_f_formation_id, player_id}, id)
    end)
  end

  def get_f_formation_speaker_id(f_formation_id) do
    ConCache.get(cache(), {:f_formation_speaker, f_formation_id})
  end

  def get_f_formation_speaker(f_formation_id) do
    f_formation_id
    |> get_f_formation_speaker_id()
    |> get_player()
  end

  def put_f_formation_speaker_id(f_formation_id, player_id) do
    ConCache.put(cache(), {:f_formation_speaker, f_formation_id}, player_id)
  end

  def get_autopilots(f_formation_id) do
    cache()
    |> ConCache.ets()
    |> :ets.match({{:autopilot, :"$1"}, f_formation_id})
    |> Enum.map(fn [player_id] -> player_id end)
    |> _touch()  # Necessary for keeping the data in cache
  end

  def put_autopilot(player_id, f_formation_id) do
    ConCache.put(cache(), {:autopilot, player_id}, f_formation_id)
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
