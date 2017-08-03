defmodule UnsocialVR.HTTP.Scores do
  @moduledoc """
  A browser directed HTTP endpoint for reporting and reseting scores.
  """

  use Plug.Router

  require EEx

  EEx.function_from_file(
    :def,
    :template,
    "lib/unsocial_vr/templates/scores.html.eex",
    [:scores]
  )

  plug Plug.Logger, log: :debug
  plug :match
  plug :dispatch

  get "/" do
    scores = UnsocialVR.Scores.scores()

    resp(conn, 200, template(scores))
  end

  post "/reset" do
    UnsocialVR.Scores.reset()

    # Redirect to "/scores"
    conn
    |> Plug.Conn.put_resp_header("location", "/scores")
    |> Plug.Conn.resp(302, "You are being redirected.")
    |> Plug.Conn.halt()
  end

  match _ do
    resp(conn, 404, "oops")
  end

end
