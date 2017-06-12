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
    ConCache.put(:cache, player_id, transform)

    # Analyze the scene
    all_players = get_players()
    side_conversations = UnsocialVR.FFormations.analyze(all_players)
    scene = UnsocialVR.SceneAnalysis.remote_players(
      player_id, all_players, side_conversations
    )

    # Reply with scenen analysis
    {:ok, scene} = Poison.encode(scene)
    resp(conn, 200, scene)
  end

  match _ do
    resp(conn, 404, "oops")
  end

  def get_players() do
    :cache
    |> ConCache.ets()
    |> :ets.tab2list()
    |> Enum.map(fn {id, player_data} -> Map.put(player_data, :id, id) end)
  end

end
