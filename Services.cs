﻿using Amazon.S3;
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

        public static ReadCommentDto MapToReadCommentDto(Comments comment, IPostRepo postRepo, IUserRepo userRepo){
            var user = userRepo.GetUserById(comment.UserId);

            return new ReadCommentDto(){
                Id = comment.Id,
                PostId = comment.PostId,
                UserId = comment.UserId,
                UserName = user.Name,
                UserImagePath = user.ImagePath,
                Body = comment.Body,
                DatePosted = comment.DatePosted
            };
        }

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

        public static ReadMessageDto MapToReadMessageDto(Message message){
            return new ReadMessageDto()
            {
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                MessageContent = message.MessageContent,
                DateSent = message.DateSent
            };
        }

        /*
            This function takes a post and map it to a post dto.
        */
        public static ReadPostDto MapToReadPostDto(Post post, IPostRepo postRepo, IUserRepo userRepo)
        {
            var comments = postRepo.GetPostComments(post.Id);
            var dtos = new List<ReadCommentDto>();
            foreach(var comment in comments)
                dtos.Add(Services.MapToReadCommentDto(comment, postRepo, userRepo));
            
            return new ReadPostDto()
            {
                Id = post.Id,
                User = Services.MapToReadUserDto(userRepo.GetUserById(post.UserId),userRepo),
                Description = post.Description,
                ImagePath = post.ImagePath,
                DatePosted = post.DatePosted,
                likes = postRepo.GetLikes(post.Id),
                Comments = dtos
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

        public static Comments MapToComment(WriteCommentsDto dto){
            return new Comments()
            {
                UserId = dto.UserId,
                PostId = dto.PostId,
                Body = dto.Body,
                DatePosted = dto.DatePosted
            };
        }

        public static Message MapToMessage(WriteMessageDto dto){
            return new Message()
            {
                SenderId = dto.SenderId,
                ReceiverId = dto.ReceiverId,
                MessageContent = dto.MessageContent,
                DateSent = dto.DateSent
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
