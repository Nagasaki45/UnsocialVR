defmodule UnsocialVR.HTTP do
  @moduledoc """
  An HTTP endpoint for the social behaviour server.
  """

  use Plug.Router

  require Logger

  plug :match
  plug :dispatch

  post "/:player_id" do
    # Input parsing
    player_id = String.to_integer(player_id)
    {:ok, body, conn} = read_body(conn)
    %{"transform" => transform} = URI.decode_query(body)
    {:ok, transform} = Poison.decode(transform)

    UnsocialVR.SceneAnalysis.cache_player(player_id, transform)

    # Reply with scenen analysis
    scene = UnsocialVR.SceneAnalysis.remote_players(player_id)
    {:ok, scene} = Poison.encode(scene)
    resp(conn, 200, scene)
  end

  get "/:player_id/autopilot/start" do
    player_id
    |> String.to_integer()
    |> UnsocialVR.SceneAnalysis.start_autopilot()

    logged_gotcha(conn)
  end

  get "/:player_id/autopilot/stop" do
    player_id
    |> String.to_integer()
    |> UnsocialVR.SceneAnalysis.stop_autopilot()

    logged_gotcha(conn)
  end

  # The next endpoints do nothing but logging the interaction.

  get "/:player_id/participant-id/:participant_id" do
    logged_gotcha(conn)
  end

  get "/:player_id/collect-token" do
    logged_gotcha(conn)
  end

  get "/:player_id/accuse/:other_player_id/:correctness" do
    logged_gotcha(conn)
  end

  get "/:player_id/talking/:status" do
    logged_gotcha(conn)
  end

  match _ do
    resp(conn, 404, "oops")
  end

  def logged_gotcha(conn) do
    Logger.info([conn.method, ?\s, conn.request_path])
    resp(conn, 200, "Gotcha!")
  end

end
