defmodule Server.HTTP do
  use Plug.Router
  require Logger

  plug :match
  plug :dispatch

  get "/:player_id/:requester_id" do
	{:ok, transform} = Server.PlayersStash.fetch(player_id)
    resp(conn, 200, transform)
  end

  post "/:player_id" do
    {:ok, body, conn} = read_body(conn)
	%{"transform" => transform} = URI.decode_query(body)
	Server.PlayersStash.put(player_id, transform)
    send_resp(conn, 200, "Got your transform!")
  end

end