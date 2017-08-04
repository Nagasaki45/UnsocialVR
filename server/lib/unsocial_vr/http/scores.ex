defmodule UnsocialVR.HTTP.Scores do
  @moduledoc """
  A browser directed HTTP endpoint for reporting and reseting scores.
  """

  use Plug.Router

  require EEx
  require Logger

  EEx.function_from_file(
    :def,
    :template,
    "lib/unsocial_vr/templates/scores.html.eex",
    [:scores]
  )

  plug :match
  plug :dispatch

  get "/" do
    scores = UnsocialVR.Scores.scores()

    resp(conn, 200, template(scores))
  end

  post "/reset-scores" do
    UnsocialVR.Scores.reset_scores()
    redirect_to_scores(conn)
  end

  post "/reset-hard" do
    UnsocialVR.Scores.reset_hard()
    redirect_to_scores(conn)
  end

  match _ do
    resp(conn, 404, "oops")
  end

  def redirect_to_scores(conn) do
    Logger.info([conn.method, ?\s, conn.request_path])
    conn
    |> Plug.Conn.put_resp_header("location", "/scores")
    |> Plug.Conn.resp(302, "You are being redirected.")
    |> Plug.Conn.halt()
  end

end
