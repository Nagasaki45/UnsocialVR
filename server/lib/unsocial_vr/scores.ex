defmodule UnsocialVR.Scores do
  @moduledoc """
  Track the participants score.
  """

  use GenServer

  # INTERFACE

  def start_link() do
    GenServer.start_link(__MODULE__, %{}, name: __MODULE__)
  end

  def add(player_id, ammount) do
    GenServer.cast(__MODULE__, {:add, player_id, ammount})
  end

  def scores() do
    GenServer.call(__MODULE__, :scores)
  end

  def reset() do
    GenServer.cast(__MODULE__, :reset)
  end

  def register_participant(player_id, participant_id) do
    GenServer.cast(
      __MODULE__,
      {:register_participant, player_id, participant_id}
    )
  end

  # CALLBACKS

  def handle_cast({:add, player_id, ammount}, scores) do
    scores = Map.update(
      scores,
      player_id,
      %{participant_id: nil, score: 1},  # init value
      &(modify_score(&1, ammount))  # update func
    )
    {:noreply, scores}
  end

  def handle_cast(:reset, _old_data) do
    {:noreply, %{}}
  end

  def handle_cast({:register_participant, player_id, participant_id}, scores) do
    player_data = %{participant_id: participant_id, score: 0}
    scores = Map.put(scores, player_id, player_data)
    {:noreply, scores}
  end

  def handle_call(:scores, _from, scores) do
    {:reply, scores, scores}
  end

  # INTERNALS

  def modify_score(%{score: current} = data, change) do
    new = if current + change < 0, do: 0, else: current + change
    %{data | score: new}
  end

end
