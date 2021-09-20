using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using OOP_WORKSHOP_PROJECT.Data;
using OOP_WORKSHOP_PROJECT.Dtos;
using OOP_WORKSHOP_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace OOP_WORKSHOP_PROJECT
{
    public abstract class Services
    {
        private const string bucketName = "oopbucket";
        private const string baseObjURL = @"https://oopbucket.s3.us-east-2.amazonaws.com/";

        public static ReadUserDto MapToReadUserDto(User user, IUserRepo repo)
        {
            return new ReadUserDto()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                ImagePath = user.ImagePath,
                Followers = (List<int>)repo.GetFollowers(user.Id),
                Following = (List<int>)repo.GetFollowing(user.Id)
            };
        }

        public static User MapToUser(WriteUserDto dto)
        {
            return new User()
            {
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Name = dto.Name,
                ImagePath = dto.ImagePath
            };
        }

        public static string SaveImage(IFormFile file, string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string newFilePath = path + DateTime.Now.GetHashCode() + file.FileName;
            using (FileStream fileStream = System.IO.File.Create(newFilePath))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            return newFilePath;
        }

        public static async Task<string> SaveImageAWSAsync(string file)
        {
            string newFilePath = DateTime.Now.GetHashCode() + file;

            var client = new AmazonS3Client();

            PutObjectRequest putRequest = new PutObjectRequest
            {
                BucketName = "oopbucket",
                Key = newFilePath,
                FilePath = file
            };

            PutObjectResponse response = await client.PutObjectAsync(putRequest);
            
            return baseObjURL + UrlEncode(newFilePath);
        }

        /**
         * This method will encode the file key in S3 the way aws encodes it
         * */
        public static string UrlEncode(string str)
        {
            string encoded = "";

            for (int i = 0; i < str.Length; i++)
            {
                switch (str[i])
                {
                    case '!':
                        encoded += "%21";
                        break;
                    case '#':
                        encoded += "%23";
                        break;
                    case '$':
                        encoded += "%24";
                        break;
                    case '&':
                        encoded += "%26";
                        break;
                    case '\'':
                        encoded += "%27";
                        break;
                    case '(':
                        encoded += "%28";
                        break;
                    case ')':
                        encoded += "%29";
                        break;
                    case '*':
                        encoded += "%2A";
                        break;
                    case '+':
                        encoded += "%2B";
                        break;
                    /*case '.':
                        encoded += "%2C";
                        break;*/
                    case '/':
                        encoded += "%2F";
                        break;
                    case '\\':
                        encoded += "%5C";
                        break;
                    case ':':
                        encoded += "%3A";
                        break;
                    case ';':
                        encoded += "%3B";
                        break;
                    case '=':
                        encoded += "%3D";
                        break;
                    case '?':
                        encoded += "%3F";
                        break;
                    case '@':
                        encoded += "%40";
                        break;
                    case '[':
                        encoded += "%5B";
                        break;
                    case ']':
                        encoded += "%5D";
                        break;
                    default:
                        encoded += str[i];
                        break;
                }
            }
            return encoded;
        }
    }
}
