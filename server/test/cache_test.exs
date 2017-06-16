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
    add_autopilots(1, [2])  # player 1 is faking towared player 2
    assert get_autopilots(1) == []
    assert get_autopilots(2) == [1]
    remove_autopilots(1, [2])
    assert get_autopilots(1) == []
    assert get_autopilots(2) == []
  end

  test "partially remove autopilots" do
    assert get_autopilots(1) == []
    assert get_autopilots(2) == []
    assert get_autopilots(3) == []
    add_autopilots(1, [2, 3])  # player 1 is faking towared 2 players
    remove_autopilots(1, [2])
    assert get_autopilots(1) == []
    assert get_autopilots(2) == []
    assert get_autopilots(3) == [1]
    remove_autopilots(1, [3])
    assert get_autopilots(1) == []
    assert get_autopilots(2) == []
    assert get_autopilots(3) == []
  end

  test "remove autopilot towards someone that wasn't faked" do
    assert get_autopilots(1) == []
    assert get_autopilots(2) == []
    assert get_autopilots(3) == []
    add_autopilots(1, [2])
    remove_autopilots(1, [2, 3])
    assert get_autopilots(1) == []
    assert get_autopilots(2) == []
    assert get_autopilots(3) == []
  end
end
