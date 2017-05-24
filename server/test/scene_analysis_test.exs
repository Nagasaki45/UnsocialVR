defmodule SceneAnalysisTest do
  use ExUnit.Case
  doctest UnsocialVR.SceneAnalysis

  import UnsocialVR.SceneAnalysis

  test "empty attention map result in empty attention chain" do
    talker = 1
    assert attention_chain(talker, %{}) == MapSet.new([talker])
  end

  test "single attention chain" do
    attention_map = %{
      1 => 2,
      2 => 3,
    }
    assert attention_chain(3, attention_map) == MapSet.new([1, 2, 3])
  end

  test "broken attention chains" do
    attention_map = %{
      1 => 2,
      2 => 1,
      3 => 4,
    }
    assert attention_chain(1, attention_map) == MapSet.new([1, 2])
    assert attention_chain(4, attention_map) == MapSet.new([3, 4])
  end

  test "merge overlapping sets" do
    sets = [MapSet.new([1, 2]), MapSet.new([2, 3])]
    assert merge_overlapping_sets(sets) == [MapSet.new([1, 2, 3])]
  end

  test "no overlapping sets" do
    sets = [MapSet.new([1, 2]), MapSet.new([3, 4])]
    assert merge_overlapping_sets(sets) == sets
  end

  test "not all sets overlap" do
    sets = [MapSet.new([1, 2]), MapSet.new([4, 5]), MapSet.new([2, 3])]
    expected = [MapSet.new([1, 2, 3]), MapSet.new([4, 5])]
    assert merge_overlapping_sets(sets) == expected
  end

  test "no players scenario" do
    assert analyze_side_conversations(%{}) == []
  end

  test "no speakers scenario" do
    players = %{
      1 => %{"isTalking" => false, "attentionTo" => nil},
    }
    assert analyze_side_conversations(players) == []
  end

  test "one speaker scenario" do
    players = %{
      1 => %{"isTalking" => true, "attentionTo" => nil},
      2 => %{"isTalking" => false, "attentionTo" => 1},
      3 => %{"isTalking" => false, "attentionTo" => 1},
    }
    assert analyze_side_conversations(players) == [MapSet.new([1, 2, 3])]

    players = %{
      1 => %{"isTalking" => true, "attentionTo" => 2},
      2 => %{"isTalking" => false, "attentionTo" => 1},
      3 => %{"isTalking" => false, "attentionTo" => 1},
    }
    assert analyze_side_conversations(players) == [MapSet.new([1, 2, 3])]

    players = %{
      1 => %{"isTalking" => true, "attentionTo" => 3},
      2 => %{"isTalking" => false, "attentionTo" => 1},
      3 => %{"isTalking" => false, "attentionTo" => 1},
    }
    assert analyze_side_conversations(players) == [MapSet.new([1, 2, 3])]
  end

  test "teacher scenario with side conversation" do
    players = %{
      1 => %{"isTalking" => true, "attentionTo" => 4},  # The teacher
      2 => %{"isTalking" => true, "attentionTo" => 3},  # Unattended student
      3 => %{"isTalking" => false, "attentionTo" => 2},  # Unattended student
      4 => %{"isTalking" => false, "attentionTo" => 1},  # Attended student
      5 => %{"isTalking" => false, "attentionTo" => 1},  # Attended student
    }
    expected = [MapSet.new([1, 4, 5]), MapSet.new([2, 3])]
    assert analyze_side_conversations(players) == expected
  end

  test "multiple speakers one conversation scenario" do
    players = %{
      1 => %{"isTalking" => true, "attentionTo" => 3},
      2 => %{"isTalking" => true, "attentionTo" => 1},
      3 => %{"isTalking" => false, "attentionTo" => 2},
    }
    assert analyze_side_conversations(players) == [MapSet.new([1, 2, 3])]
  end

  test "two parallel conversations scenario" do
    players = %{
      1 => %{"isTalking" => true, "attentionTo" => 2},
      2 => %{"isTalking" => false, "attentionTo" => 1},
      3 => %{"isTalking" => true, "attentionTo" => 4},
      4 => %{"isTalking" => false, "attentionTo" => 3},
    }
    expected = [MapSet.new([1, 2]), MapSet.new([3, 4])]
    assert analyze_side_conversations(players) == expected
  end
end
