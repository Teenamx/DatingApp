using Api.DTO;
using Api.Entities;
using Api.Helpers;
using Api.Interface;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public MessageRepository(DataContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void AddGroup(Group group)
        {
            context.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
            context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            context.Messages.Remove(message);
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await context.Connections.FindAsync(connectionId);
        }

        public async Task<Group> GetGroupForConnection(string connectionId)
        {
            return await context.Groups
                .Include(c => c.Connections)
                .Where(c => c.Connections.Any(x => x.ConnectionId == connectionId))
                .FirstOrDefaultAsync();
        }

        public async Task<Message> GetMessage(int id)
        {
            return await context.Messages.Include(u =>u.Sender).Include(u=>u.Recipient).SingleOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<PagedList<MessageDto>> GetMessageForUser(MessageParams messageParams)
        {
            var query = context.Messages
                .OrderByDescending(m => m.MessageSent)
                .ProjectTo<MessageDto>(mapper.ConfigurationProvider)
                .AsQueryable();
            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.RecipientUsername == messageParams.UserName && u.RecipientDeleted==false),
                "Outbox" => query.Where(u => u.SenderUsername == messageParams.UserName && u.SenderDeleted==false),
                _ => query.Where(u => u.RecipientUsername == messageParams.UserName && u.RecipientDeleted==false && u.DateRead == null)
            };

          

            return await PagedList<MessageDto>.CreateAsync(query, messageParams.PageNumber, messageParams.PageSize);

        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await context.Groups.Include(x => x.Connections).FirstOrDefaultAsync(x => x.Name == groupName);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
        {

            var messages = await context.Messages
                         .Where(m => m.Recipient.UserName == currentUsername
                         && m.Sender.UserName == recipientUsername && m.RecipientDeleted==false
                         || m.Recipient.UserName == recipientUsername
                         && m.Sender.UserName == currentUsername && m.SenderDeleted==false
                         )
                         .OrderBy(m => m.MessageSent)
                         .ProjectTo<MessageDto>(mapper.ConfigurationProvider)
                        .ToListAsync();
            var unreadMessages = messages.Where(m => m.DateRead == null && m.RecipientUsername == currentUsername).ToList();
            if(unreadMessages.Any())
            {
               foreach(var message in unreadMessages)
                {
                    message.DateRead = DateTime.UtcNow;
                }

               // await context.SaveChangesAsync();

            }

            return messages;

        }

        public void RemoveConnection(Connection connection)
        {
            context.Connections.Remove(connection);
        }

      
    }
}
