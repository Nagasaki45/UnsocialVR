defmodule UnsocialVR.HTTP do
  use Plug.Router

  plug Plug.Logger
  plug :match
  plug :dispatch

  post "/:player_id" do
    {:ok, body, conn} = read_body(conn)
    %{"transform" => transform} = URI.decode_query(body)
    {:ok, transform} = Poison.decode(transform)
    UnsocialVR.PlayersStash.put(player_id, transform)
    scene = UnsocialVR.SceneAnalysis.remote_players(player_id)
    {:ok, scene} = Poison.encode(scene)
    resp(conn, 200, scene)
  end

  match _ do
    resp(conn, 404, "oops")
  end

end
