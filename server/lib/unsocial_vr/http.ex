defmodule UnsocialVR.HTTP do
  use Plug.Router

  plug Plug.Logger
  plug :match
  plug :dispatch

  post "/:player_id" do
    # Input parsing
    player_id = String.to_integer(player_id)
    {:ok, body, conn} = read_body(conn)
    %{"transform" => transform} = URI.decode_query(body)
    {:ok, transform} = Poison.decode(transform)

    # Upload player data
    ConCache.put(:cache, player_id, transform)

    # Reply with scenen analysis
    all_players = ConCache.ets(:cache) |> :ets.tab2list() |> Enum.into(%{})
    scene = UnsocialVR.SceneAnalysis.remote_players(player_id, all_players)
    {:ok, scene} = Poison.encode(scene)
    resp(conn, 200, scene)
  end

  match _ do
    resp(conn, 404, "oops")
  end

end
