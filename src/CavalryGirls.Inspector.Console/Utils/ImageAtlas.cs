using CavalryGirls.Inspector.Models;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace CavalryGirls.Inspector.Utils;

public static class ImageAtlas
{
    private static readonly ImageEncoder _imageEncoder = new WebpEncoder
    {
        Quality = 90, FileFormat = WebpFileFormatType.Lossy
    };

    public static async Task<Atlas> Create(string[] imagePaths, string outputPath, string outputFileName)
    {
        outputPath.CreateFolderIfRequired();

        var path = Path.Combine(outputPath, outputFileName);
        var (rows, columns) = GetAtlasSize(imagePaths.Length);

        var images = await LoadImages(imagePaths);
        var imageSize = GetMinImageSize(images);
        var (atlasWidth, atlasHeight) = (rows * imageSize, columns * imageSize);

        using var atlas = new Image<Rgba32>(atlasWidth, atlasHeight, Color.Transparent);
        for (var i = 0; i < images.Length; i++)
        {
            ResizeImage(images[i], imageSize);

            var (index, x, y) = (i, i % columns * imageSize, i / columns * imageSize);
            atlas.Mutate(context => context.DrawImage(images[index], new Point(x, y), 1f));
        }

        await atlas.SaveAsync(path, _imageEncoder);
        DisposeImages(images);

        return new Atlas(rows, columns, imageSize);
    }

    private static void ResizeImage(Image<Rgba32> image, int size)
    {
        var aspectRatio = (double)image.Width / image.Height;
        var (width, height) = image.Width > image.Height
            ? (size, (int)Math.Round(size / aspectRatio))
            : ((int)Math.Round(size * aspectRatio), size);

        image.Mutate(context => context.Resize(width, height));
    }

    private static int GetMinImageSize(Image<Rgba32>[] images)
    {
        var sizes = new HashSet<(int Width, int Height)>();
        foreach (var image in images)
        {
            sizes.Add((image.Width, image.Height));
        }

        var (width, height) = sizes.Min();
        return width < height ? width : height;
    }

    private static async Task<Image<Rgba32>[]> LoadImages(string[] imagePaths)
    {
        var images = new Image<Rgba32>[imagePaths.Length];
        for (var i = 0; i < imagePaths.Length; i++)
        {
            images[i] = await Image.LoadAsync<Rgba32>(imagePaths[i]);
        }

        return images;
    }

    private static void DisposeImages(Image<Rgba32>[] images)
    {
        foreach (var image in images)
        {
            image.Dispose();
        }
    }

    private static (int Rows, int Columns) GetAtlasSize(int numberOfImages)
    {
        var sqrtN = (int)Math.Ceiling(Math.Sqrt(numberOfImages));
        var rows = sqrtN;
        var columns = (int)Math.Ceiling((double)numberOfImages / rows);

        return (rows, columns);
    }
}