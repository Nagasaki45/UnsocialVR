defmodule Server.Application do
  use Application

  def start(_type, _args) do
    import Supervisor.Spec, warn: false

    children = [
	  worker(Server.PlayersStash, []),
      Plug.Adapters.Cowboy.child_spec(:http, Server.HTTP, [], port: 8080)
    ]

    opts = [strategy: :one_for_one, name: Server.Supervisor]
    Supervisor.start_link(children, opts)
  end
end
