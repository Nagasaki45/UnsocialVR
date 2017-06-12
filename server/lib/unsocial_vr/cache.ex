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

  def put(player_id, data) do
    ConCache.put(:cache, player_id, data)
  end
end
