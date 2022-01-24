using System;
using System.IO;
using System.Runtime.InteropServices;
using SDL2;
using StbImageSharp;

namespace SkylineEngine
{
    public static class Image
    {
        public static byte[] Load2(string filePath, out int width, out int height)
        {
            IntPtr image = SDL_image.IMG_Load(filePath);
            
            if(image == IntPtr.Zero)		
            {
                Console.WriteLine("Could not load image: " + filePath);
                width = 0;
                height = 0;
                return null;
            }
            
            SDL_Surface surface = Marshal.PtrToStructure<SDL_Surface>(image);
            SDL_PixelFormat format = Marshal.PtrToStructure<SDL_PixelFormat>(surface.pixelFormat);

            width = surface.w;
            height = surface.h;
            int index = 0;
            int size = sizeof(uint);
            byte[] imageData = new byte[width*height*size];            
            
            SDL.SDL_LockSurface(image);
            
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    uint pixel = GetPixel(surface.pixels, format.BitsPerPixel, surface.pitch, x, y);			
                    SDL_Color color;			
                    SDL.SDL_GetRGBA(pixel, surface.pixelFormat, out color.r, out color.g, out color.b, out color.a);			
                    imageData[index] = color.r;
                    imageData[index+1] = color.g;
                    imageData[index+2] = color.b;
                    imageData[index+3] = color.a;
                    index += size;
                }
            }	
            
            SDL.SDL_UnlockSurface(image);		
            SDL.SDL_FreeSurface(image);	
            return imageData;
        }

        public static byte[] Load(string filePath, out int width, out int height)
        {
            if (!System.IO.File.Exists(filePath))
                throw new System.Exception("File does not exist: " + filePath);

            byte[] imageData = null;

            using (var stream = File.OpenRead(filePath))
            {
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                imageData = image.Data;
                width = image.Width;
                height = image.Height;
            }

            return imageData;
        }

        private static unsafe uint GetPixel(IntPtr pixels, int bpp, int pitch, int x, int y)
        {            
            /* Here p is the address to the pixel we want to retrieve */

            byte* ptr = (byte*)pixels;
            byte *p = ptr + y * pitch + x * bpp;

            switch (bpp)
            {
            case 1:
                return *p;

            case 2:
                return *(ushort*)p;

            case 3:
                if (!BitConverter.IsLittleEndian)
                {
                    return (uint)(p[0] << 16 | p[1] << 8 | p[2]);
                }
                else
                {
                    return (uint)(p[0] | p[1] << 8 | p[2] << 16);
                }

                case 4:
                    return *(uint*)p;

                default:
                    return 0;       /* shouldn't happen, but avoids warnings */
            }
        }
    }
}