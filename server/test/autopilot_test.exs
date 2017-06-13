defmodule AutopilotTest do
  use ExUnit.Case
  doctest UnsocialVR.Autopilot

  import UnsocialVR.Autopilot

  test "character shift" do
    data = %{"chestPosition" => %{"x" => 1, "y" => 1, "z" => 1}}
    result = shift_character(data, {1, 2})
    assert result["chestPosition"]["x"] == 2
    assert result["chestPosition"]["y"] == 1
    assert result["chestPosition"]["z"] == 3
  end

  test "finilize recording shift chestPosition to origin" do
    data = [
      {0, %{"chestPosition" => %{"x" => 1, "y" => 1, "z" => 1}}},
      {1, %{"chestPosition" => %{"x" => 2, "y" => 2, "z" => 2}}},
    ]
    expected = [
      {0, %{"chestPosition" => %{"x" => -0.5, "y" => 1, "z" => -0.5}}},
      {1, %{"chestPosition" => %{"x" => 0.5, "y" => 2, "z" => 0.5}}},
    ]
    assert finilize_recording(data) == expected
  end

end
