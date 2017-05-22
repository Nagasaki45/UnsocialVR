defmodule UnsocialVR.SceneAnalysis do

  def remote_players(local_id) do
    keys = UnsocialVR.PlayersStash.keys()
    keys
    |> Stream.filter(fn key -> key != local_id end)
    |> Enum.map(fn key -> remote_player(local_id, key) end)
  end

  def remote_player(_local_id, remote_id) do
    {:ok, transform} = UnsocialVR.PlayersStash.fetch(remote_id)
    Map.put(transform, :id, remote_id)
  end
end
