defmodule UnsocialVR.Participants do
  @moduledoc """
  Keeps a map between player ID and participant ID.
  """

  use GenServer

  def start_link() do
    GenServer.start_link(__MODULE__, %{}, name: __MODULE__)
  end

  def put(player_id, participant_id) do
    GenServer.cast(__MODULE__, {:put, player_id, participant_id})
  end

  def get(player_id) do
    GenServer.call(__MODULE__, {:get, player_id})
  end

  def handle_cast({:put, player_id, participant_id}, map) do
    {:noreply, Map.put(map, player_id, participant_id)}
  end

  def handle_call({:get, player_id}, _from, map) do
    {:reply, Map.get(map, player_id), map}
  end
end
