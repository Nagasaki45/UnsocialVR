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

  def get_autopilots(f_formation_id) do
    case ConCache.get(cache(), {:autopilots, f_formation_id}) do
      nil -> []
      otherwise -> otherwise
    end
  end

  def add_autopilot(f_formation_id, player_id) do
    ConCache.update(cache(), {:autopilots, f_formation_id}, fn autopilots ->
      autopilots = autopilots || []
      {:ok, [player_id | autopilots]}
    end)
  end

  def remove_autopilot(f_formation_id, player_id) do
    ConCache.update(cache(), {:autopilots, f_formation_id}, fn autopilots ->
      {:ok, Enum.filter(autopilots, fn id -> id != player_id end)}
    end)
  end
end
