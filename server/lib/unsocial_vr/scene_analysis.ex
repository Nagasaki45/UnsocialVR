defmodule UnsocialVR.SceneAnalysis do

  @doc """
  How I see the other players.
  """
  def remote_players(local_id) do
    all_players = UnsocialVR.Cache.get_players()
    me = get_me(local_id, all_players)
    f_formations = UnsocialVR.FFormations.get()
    the_others = Enum.filter(all_players, fn player -> player != me end)
    the_others
    |> player_perspective(me, f_formations)
    |> Stream.map(&set_default(&1, :state, :real))
    |> Enum.map(&apply_autopilot/1)
  end

  def get_me(local_id, all_players) do
    all_players
    |> Enum.filter(fn player_data -> player_data.id == local_id end)
    |> hd
  end

  @doc """
  If the state of the player is autopilot, replace its positions
  with autopilot positions
  """
  def apply_autopilot(%{state: :autopilot, id: id} = player_data) do
    head_position = player_data["headPosition"]
    position_shift = {head_position["x"], head_position["z"]}
    time_shift = id * 4321  # Just to cause difference between players
    autopilot_data = UnsocialVR.Autopilot.play(position_shift, time_shift)
    Map.merge(player_data, autopilot_data)
  end
  def apply_autopilot(non_autopilot_player_data) do
    non_autopilot_player_data
  end

  @large_f_formation 2

  @doc """
  How the player see the world, based on his data, the other players,
  and f-formation analysis.

  - If there are less than 2 f-formations or me not participating
    in any f-formation reply real state.
  - The players that are not in my f-formation:
    - The player I'm talking too: autopilot.
    - If I'm a speaker and f-formation is large: autopilot.
    - Otherwise: ignored.
  """
  def player_perspective(the_others, _me, f_formations)
  when length(f_formations) < 2 do
    the_others
  end
  def player_perspective(the_others, me, f_formations) do
    case Enum.find(f_formations, &Enum.member?(&1, me.id)) do
      nil ->
        the_others
      f_formation ->
        participate_perspective(the_others, me, f_formation)
    end
  end

  def participate_perspective(the_others, me, f_formation) do
    {attendies, the_rest} = group_by(the_others, &Enum.member?(f_formation, &1.id))
    large_f_formation? = length(f_formation) > @large_f_formation
    the_rest = Enum.map(the_rest, fn other ->
      state = cond do
        me["isTalking"] && me["attentionTo"] == other.id -> :autopilot
        me["isTalking"] && large_f_formation? -> :autopilot
        true -> :ignored
      end
      Map.put(other, :state, state)
    end)
    attendies ++ the_rest
  end

  # Some simple and general utility functions

  defp set_default(map, key, default) do
    Map.put(map, key, Map.get(map, key, default))
  end

  defp group_by(enumerable, function) do
    grouped = Enum.group_by(enumerable, function)
    {Map.get(grouped, true, []), Map.get(grouped, false, [])}
  end

end
