
using WhatIf.Models;

namespace WhatIf.ViewModels
{
    public class XkcdComicVM
    {
        public int IdComic { get; set; } 
        public XkcdComic Comic { get; set; }
        public int NextId { get; set; }
        public int PrevId { get; set; }
    }
}