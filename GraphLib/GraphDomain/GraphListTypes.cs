﻿using System.Collections;
using DiscreteMathLab3.GraphDomain.GraphTypes;

namespace DiscreteMathLab3.GraphDomain
{
    public abstract class GraphList<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>
    {
        protected T[] Items { get; set; }

        public int Count { get { return Items.Length; } }

        public GraphList(T[] items)
        {
            Items = items;
        }


        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Items.Length)
                    throw new IndexOutOfRangeException("Индекс(ы) выходит за пределы списка");

                return Items[index];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Items.Length; i++)
            {
                yield return Items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public record RelationListItem(Node Node, Node[] Neighbors);

    public class RelationshipLists : GraphList<RelationListItem>
    {
        public RelationshipLists(RelationListItem[] vertexts) : base(vertexts) { }

    }

    public record IncidentsListsItem(Edge Edge, Node[] Neighbors);

    public class IncidentsLists : GraphList<IncidentsListsItem>
    {
        public IncidentsLists(IncidentsListsItem[] items) : base(items) { }

    }

}