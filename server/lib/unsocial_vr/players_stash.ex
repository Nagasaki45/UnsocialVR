defmodule UnsocialVR.PlayersStash do
	use GenServer

  def start_link() do
	GenServer.start_link(__MODULE__, Map.new(), name: __MODULE__)
  end

  def put(key, value) do
	GenServer.cast(__MODULE__, {:put, key, value})
  end

  def fetch(key) do
    GenServer.call(__MODULE__, {:fetch, key})
  end

  def keys() do
    GenServer.call(__MODULE__, {:keys})
  end

  def handle_cast({:put, key, value}, stash) do
    {:noreply, Map.put(stash, key, value)}
  end

  def handle_call({:fetch, key}, _from, stash) do
    {:reply, Map.fetch(stash, key), stash}
  end

  def handle_call({:keys}, _from, stash) do
    {:reply, Map.keys(stash), stash}
  end

end
