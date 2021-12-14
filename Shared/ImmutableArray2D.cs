namespace System.Collections.Immutable
{
    public class ImmutableArray2D<T> : IEnumerable<T>
    {
        private readonly IImmutableList<T> innerArray;

        public ImmutableArray2D(IEnumerable<T> source, int width, int height)
        {
            if (width <= 0)
                throw new ArgumentException("must be greater than 0.", nameof(width));
            if (width <= 0)
                throw new ArgumentException("must be greater than 0.", nameof(height));
            if (source.Count() != width * height)
                throw new ArgumentException($"wrong number of elements for given '{nameof(width)}' and '{nameof(height)}'.", nameof(source));
            
            Width = width;
            Height = height;
            innerArray = source.ToImmutableArray();
        }
        public ImmutableArray2D(ImmutableArray2D<T> source)
            : this(source, source.Width, source.Height)
        { }

        public ImmutableArray2D(T fillValue, int width, int height)
            : this(Enumerable.Repeat(fillValue, width * height), width, height)
        { }


        public int Width { get; }
        public int Height { get; }


        public T this[int x, int y] =>
            innerArray[ToInnerCoordinates(x, y)];

        public T this[(int x, int y) coords] =>
            this[coords.x, coords.y];

        public IImmutableList<T> Row(int y) =>
            innerArray.Chunk(Width)
                .Skip(y)
                .First()
                .ToImmutableArray();

        public IImmutableList<T> Column(int x) =>
            innerArray.Chunk(Width)
                .Select(chunk => chunk.Skip(x).First())
                .ToImmutableArray();


        public ImmutableArray2D<T> SetItem(int x, int y, T value) =>
            new(
                innerArray.SetItem(ToInnerCoordinates(x, y), value),
                Width,
                Height
            );

        private int ToInnerCoordinates(int x, int y)
        {
            if (x < 0 || x >= Width)
                throw new ArgumentOutOfRangeException(nameof(x));
            if (y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException(nameof(y));

            return y * Width + x;
        }

        public (int x, int y) ToCoords(int i)
        {
            if (i >= innerArray.Count)
                throw new ArgumentOutOfRangeException(nameof(i));

            var x = i % Width;
            var y = (i - x) / Width;

            return (x, y);
        }

        public IEnumerator<T> GetEnumerator()
            => innerArray.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => innerArray.GetEnumerator();
    }
}