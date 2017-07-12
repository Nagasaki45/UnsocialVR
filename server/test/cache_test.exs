defmodule CacheTest do
  use ExUnit.Case
  doctest UnsocialVR.Cache

  import UnsocialVR.Cache

  setup_all do
    ConCache.start_link([], name: :cache)
    :ok
  end

  test "get players" do
    assert get_players() == []
    put_player(1, %{name: :moshe})
    assert get_players() == [%{id: 1, name: :moshe}]
  end


  test "add, get, and remove autopilots" do
    assert get_autopilots(1) == []
    assert get_autopilots(2) == []
    put_autopilot(3, 1)  # player 3 is faking towared f-formation 2
    assert get_autopilots(1) == [3]
    assert get_autopilots(2) == []
    delete_autopilot(3)
    assert get_autopilots(1) == []
    assert get_autopilots(2) == []
  end
end
