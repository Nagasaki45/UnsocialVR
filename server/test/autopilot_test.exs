defmodule AutopilotTest do
  use ExUnit.Case
  doctest UnsocialVR.Autopilot

  import UnsocialVR.Autopilot

  test "character shift" do
    data = %{"headPosition" => %{"x" => 1, "y" => 1, "z" => 1}}
    result = shift_character(data, {1, 2})
    assert result["headPosition"]["x"] == 2
    assert result["headPosition"]["y"] == 1
    assert result["headPosition"]["z"] == 3
  end

  test "finilize recording shift headPosition to origin" do
    data = [
      {0, %{"headPosition" => %{"x" => 1, "y" => 1, "z" => 1}}},
      {1, %{"headPosition" => %{"x" => 2, "y" => 2, "z" => 2}}},
    ]
    expected = [
      {0, %{"headPosition" => %{"x" => -0.5, "y" => 1, "z" => -0.5}}},
      {1, %{"headPosition" => %{"x" => 0.5, "y" => 2, "z" => 0.5}}},
    ]
    assert finilize_recording(data) == expected
  end

end
