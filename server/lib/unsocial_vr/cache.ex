defmodule UnsocialVR.Cache do
  @moduledoc """
  A single place to organize all cache operations.
  """

  def get_players() do
    :cache
    |> ConCache.ets()
    |> :ets.tab2list()
    |> Enum.map(fn {id, player_data} -> Map.put(player_data, :id, id) end)
  end

  def put_player(player_id, data) do
    ConCache.put(:cache, player_id, data)
  end

  def get_f_formations() do
    ConCache.get(:f_formations, :f_formations)
  end

  def put_f_formations(f_formations) do
    ConCache.put(:f_formations, :f_formations, f_formations)
  end
end
