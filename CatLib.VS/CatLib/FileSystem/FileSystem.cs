﻿/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using System.IO;
using CatLib.API.FileSystem;
using CatLib.Stl;

namespace CatLib.FileSystem
{
    /// <summary>
    /// 文件系统
    /// </summary>
    public sealed class FileSystem : IFileSystem
    {
        /// <summary>
        /// 文件系统适配器
        /// </summary>
        private readonly IFileSystemAdapter adapter;

        /// <summary>
        /// 文件系统
        /// </summary>
        /// <param name="adapter">适配器</param>
        public FileSystem(IFileSystemAdapter adapter)
        {
            Guard.NotNull(adapter, "adapter");
            this.adapter = adapter;
        }

        /// <summary>
        /// 文件或文件夹是否存在
        /// </summary>
        /// <param name="path">文件或文件夹是否存在</param>
        /// <returns>是否存在</returns>
        public bool Exists(string path)
        {
            return adapter.Exists(path);
        }

        /// <summary>
        /// 写入数据
        /// 如果数据已经存在则覆盖
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="contents">写入数据</param>
        /// <returns>是否成功</returns>
        public void Write(string path, byte[] contents)
        {
            adapter.Write(path, contents);
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>读取的数据</returns>
        public byte[] Read(string path)
        {
            return adapter.Read(path);
        }

        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="path">旧的文件/文件夹路径</param>
        /// <param name="newPath">新的文件/文件夹路径</param>
        /// <returns>是否成功</returns>
        public void Rename(string path, string newPath)
        {
            adapter.Rename(path, newPath);
        }

        /// <summary>
        /// 复制文件或文件夹到指定路径
        /// </summary>
        /// <param name="path">文件或文件夹路径(应该包含文件夹或者文件名)</param>
        /// <param name="copyPath">复制到的路径(不应该包含文件夹或者文件名)</param>
        /// <returns>是否成功</returns>
        public void Copy(string path, string copyPath)
        {
            adapter.Copy(path, copyPath);
        }

        /// <summary>
        /// 删除文件或者文件夹
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>是否成功</returns>
        public void Delete(string path)
        {
            adapter.Delete(path);
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <returns>是否成功</returns>
        public void CreateDir(string path)
        {
            adapter.CreateDir(path);
        }

        /// <summary>
        /// 获取文件/文件夹属性
        /// </summary>
        /// <param name="path">文件/文件夹路径</param>
        /// <returns>文件/文件夹属性</returns>
        public FileAttributes GetAttributes(string path)
        {
            return adapter.GetAttributes(path);
        }

        /// <summary>
        /// 获取文件/文件夹句柄
        /// </summary>
        /// <param name="path">文件/文件夹路径</param>
        /// <returns>文件/文件夹句柄</returns>
        public IHandler Get(string path)
        {
            if (IsDir(path))
            {
                return new Directory(this, path);
            }
            return new File(this, path);
        }

        /// <summary>
        /// 获取列表（不会迭代子文件夹）
        /// </summary>
        /// <param name="path">要获取列表的文件夹路径(如果传入的是一个文件那么将会返回文件自身路径)</param>
        /// <returns>指定目录下的文件夹句柄和文件句柄列表</returns>
        public IHandler[] GetList(string path = null)
        {
            var paths = adapter.GetList(path);
            var handlers = new IHandler[paths.Length];
            var i = 0;
            foreach (var fd in paths)
            {
                handlers[i++] = Get(fd);
            }
            return handlers;
        }

        /// <summary>
        /// 是否是文件夹
        /// </summary>
        /// <param name="path">文件/文件夹路径</param>
        /// <returns>是否是文件夹</returns>
        private bool IsDir(string path)
        {
            return (GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory;
        }
    }
}
