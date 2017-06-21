defmodule CacheTest do
  use ExUnit.Case
  doctest UnsocialVR.Cache

  import UnsocialVR.Cache

  test "get players" do
    assert get_players() == []
    put_player(1, %{name: :moshe})
    assert get_players() == [%{id: 1, name: :moshe}]
  end


  test "add, get, and remove autopilots" do
    assert get_autopilots(1) == []
    assert get_autopilots(2) == []
    add_autopilot(1, 5)  # player 5 is faking towared f-formation 1
    assert get_autopilots(1) == [5]
    assert get_autopilots(2) == []
    remove_autopilot(1, 5)
    assert get_autopilots(1) == []
    assert get_autopilots(2) == []
  end
end
