defmodule ScoresTest do
  use ExUnit.Case
  doctest UnsocialVR.Cache

  alias UnsocialVR.Scores

  setup_all do
    Scores.start_link()

    :ok
  end

  setup do
    on_exit fn ->
      Scores.reset()
    end

    :ok
  end


  test "increment new id" do
    Scores.increment(:moshe)
    assert Scores.scores().moshe.score == 1
  end

  test "decrement can't go negative" do
    Scores.increment(:moshe)
    Scores.decrement(:moshe)
    Scores.decrement(:moshe)
    assert Scores.scores().moshe.score == 0
  end

  test "reset" do
    Scores.increment(:moshe)
    Scores.increment(:yossi)
    Scores.reset()
    assert Scores.scores() == %{}
  end

  test "register_participant" do
    Scores.register_participant(:moshe, 3)
    assert Scores.scores().moshe.participant_id == 3
    assert Scores.scores().moshe.score == 0
    Scores.increment(:moshe)
    assert Scores.scores().moshe.score == 1
  end

end
