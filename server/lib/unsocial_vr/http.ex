defmodule UnsocialVR.HTTP do
  @moduledoc """
  An HTTP endpoint for the social behaviour server.
  """

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

    UnsocialVR.SceneAnalysis.cache_player(player_id, transform)

    # Reply with scenen analysis
    scene = UnsocialVR.SceneAnalysis.remote_players(player_id)
    {:ok, scene} = Poison.encode(scene)
    resp(conn, 200, scene)
  end

  get "/:player_id/start_autopilot" do
    player_id
    |> String.to_integer()
    |> UnsocialVR.SceneAnalysis.start_autopilot()
    resp(conn, 200, "Gotcha!")
  end

  get "/:player_id/stop_autopilot" do
    player_id
    |> String.to_integer()
    |> UnsocialVR.SceneAnalysis.stop_autopilot()
    resp(conn, 200, "Gotcha!")
  end

  get "/:player_id/participant-id/:participant_id" do
    player_id = String.to_integer(player_id)
    participant_id = String.to_integer(participant_id)
    UnsocialVR.Participants.put(player_id, participant_id)
    resp(conn, 200, "Gotcha!")
  end

  get "/:player_id/collect-token" do
    player_id
    |> String.to_integer()
    |> UnsocialVR.Experiment.log(:collect_token)

    resp(conn, 200, "Gotcha!")
  end

  match _ do
    resp(conn, 404, "oops")
  end

end
