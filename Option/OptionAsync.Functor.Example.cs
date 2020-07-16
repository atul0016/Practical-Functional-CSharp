﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using PracticalCSharp.Model;
using static PracticalCSharp.Model.Db;

namespace PracticalCSharp.Maybe.Functor.Example
{
 
    public class MockClientRepository
    { 
        public OptionAsync<Client> GetByIdAsync(int id)
        {
            Task<Option<Client>> r = System.Threading.Tasks.Task.Run(() =>
            {
                System.Threading.Thread.Sleep(1000);
                Option<Client> t = Clients.SingleOrDefault(x => x.Id == id);
                return t;
            });
            return r.ToAsync();
        }
    }


    public class Controller
    {
        MockClientRepository clients = new MockClientRepository();

        public Task<string> GetNameByIdAsync(int clientId) =>
            clients
            .GetByIdAsync(clientId)
            .Map(client => client.Name)
            .Match(Some: (name) => name,
                   None: () => "no client"); 
    }

    public class Demo
    { 
        public async ValueTask Run()
        {
            var assignedEmployeeName = await new Controller().GetNameByIdAsync(1); 
        }
    }
}

