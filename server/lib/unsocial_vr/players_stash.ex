defmodule UnsocialVR.PlayersStash do
	use GenServer

  def start_link() do
	GenServer.start_link(__MODULE__, Map.new(), name: __MODULE__)
  end

  def put(key, value) do
	GenServer.cast(__MODULE__, {:put, key, value})
  end

  def data() do
    GenServer.call(__MODULE__, {:data})
  end

  def handle_cast({:put, key, value}, stash) do
    {:noreply, Map.put(stash, key, value)}
  end

  def handle_call({:data}, _from, stash) do
    {:reply, stash, stash}
  end

end
