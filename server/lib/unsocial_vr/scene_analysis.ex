defmodule UnsocialVR.SceneAnalysis do

  @doc """
  How I see the other players.
  """
  def remote_players(local_id, all_players) do
    side_conversations = analyze_side_conversations(all_players)
    {me, the_others} = Map.pop(all_players, local_id)
    me = Map.put(me, :id, local_id)
    the_others
    |> Enum.map(fn {id, value} -> Map.put(value, :id, id) end)
    |> player_perspective(me, side_conversations)
    |> Enum.map(&set_default(&1, :state, :real))
  end

  @doc """
  When less than 2 players are talking everyone is in the same conversation.
  When at least 2 players are talking side coversations are formed as:
  - Each talker form a side conversation.
  - All the chains of attention that end with a talker are in the talker
    side conversation.
  - When chains overlap they merge.

  Returns a list of sets of IDs. One set per side conversation.
  """
  def analyze_side_conversations(all_players) do
    attention_map = map_values(all_players, &(&1["attentionTo"]))

    all_players
    |> Stream.filter(fn {_id, value} -> value["isTalking"] end)
    |> Stream.map(fn {id, _} -> id end)
    |> Enum.map(&attention_chain(&1, attention_map))
    |> merge_overlapping_sets()
  end

  @doc """
  The players that pay attention to the talker, either directly, or through
  other player.

  ids - A single ID (integer) or a set of IDs.
  attention_map - A map of ID -> ID of all of the non talkers attention.
  Returns a set of IDs attending to the ids in the input.
  """
  def attention_chain(id, attention_map) when is_integer(id) do
    attention_chain(MapSet.new([id]), attention_map)
  end
  def attention_chain(ids, attention_map) do
    {first_order_attendies, the_rest} = group_by(attention_map, fn {_from, to} -> to in ids end)
    updated_ids =
      first_order_attendies
      |> Stream.map(fn {from, _to} -> from end)
      |> Enum.into(ids)
    if updated_ids == ids do
      updated_ids
    else
      attention_chain(updated_ids, the_rest)
    end
  end

  @doc """
  Self explainatory, isn't it?
  """
  def merge_overlapping_sets(list_of_sets) do
    list_of_sets
    # Reduce by union with each overlapping set in the list
    |> Enum.reduce(list_of_sets, fn set, acc ->
      Enum.map(acc, fn existing_set ->
        if MapSet.disjoint?(existing_set, set) do
          existing_set
        else
          MapSet.union(existing_set, set)
        end
      end)
    end)
    |> Enum.uniq()
  end

  @large_conversation 2

  @doc """
  How the player see the world, based on his data, the other players,
  and side conversations analysis.

  - If there are less than 2 side conversations or me not participating
    in any side conversation reply real state.
  - The players that are not in my side conversation:
    - The player I'm talking too: autopilot.
    - If I'm a speaker and side conversation is large: autopilot.
    - Otherwise: ignored.
  """
  def player_perspective(the_others, _me, side_conversations)
  when length(side_conversations) < 2 do
    the_others
  end
  def player_perspective(the_others, me, side_conversations) do
    case Enum.find(side_conversations, &MapSet.member?(&1, me.id)) do
      nil ->
        the_others
      side_conversation ->
        participate_perspective(the_others, me, side_conversation)
    end
  end

  def participate_perspective(the_others, me, side_conversation) do
    {attendies, the_rest} = group_by(the_others, &MapSet.member?(side_conversation, &1.id))
    large_conversation? = MapSet.size(side_conversation) > @large_conversation
    the_rest = Enum.map(the_rest, fn other ->
      state = cond do
        me["isTalking"] && me["attentionTo"] == other.id -> :autopilot
        me["isTalking"] && large_conversation? -> :autopilot
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

  defp map_values(map, function) do
    map
    |> Stream.map(fn {key, value} -> {key, function.(value)} end)
    |> Enum.into(Map.new())
  end

  defp group_by(enumerable, function) do
    grouped = Enum.group_by(enumerable, function)
    {Map.get(grouped, true, []), Map.get(grouped, false, [])}
  end

end
