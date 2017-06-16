defmodule UnsocialVR.HTTP do
  use Plug.Router

  # plug Plug.Logger
  plug :match
  plug :dispatch

  post "/:player_id" do
    # Input parsing
    player_id = String.to_integer(player_id)
    {:ok, body, conn} = read_body(conn)
    %{"transform" => transform} = URI.decode_query(body)
    {:ok, transform} = Poison.decode(transform)

    # Upload player data
    UnsocialVR.Cache.put_player(player_id, transform)

    # Reply with scenen analysis
    scene = UnsocialVR.SceneAnalysis.remote_players(player_id)
    {:ok, scene} = Poison.encode(scene)
    resp(conn, 200, scene)
  end

  get "/:player_id/start_autopilot" do
    player_id
    |> String.to_integer()
    |> UnsocialVR.SceneAnalysis.start_autopiloting()
    resp(conn, 200, "Gotcha!")
  end

  match _ do
    resp(conn, 404, "oops")
  end

end
