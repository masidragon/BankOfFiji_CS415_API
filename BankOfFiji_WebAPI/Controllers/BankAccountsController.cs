﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BankOfFiji_WebAPI.Models;

namespace BankOfFiji_WebAPI.Controllers
{
    public class BankAccountsController : ApiController
    {
        private BankOfFijiEntities db = new BankOfFijiEntities();

        // GET: api/BankAccounts
        public IQueryable<BankAccount> GetBankAccount()
        {
            return db.BankAccount;
        }

        // GET: api/BankAccounts/5
        [ResponseType(typeof(BankAccount))]
        public IHttpActionResult GetBankAccount(int id)
        {
            BankAccount bankAccount = db.BankAccount.Find(id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            return Ok(bankAccount);
        }

        // PUT: api/BankAccounts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBankAccount(int id, BankAccount bankAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bankAccount.accountNo)
            {
                return BadRequest();
            }

            db.Entry(bankAccount).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BankAccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/BankAccounts
        [ResponseType(typeof(BankAccount))]
        public IHttpActionResult PostBankAccount(BankAccount bankAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BankAccount.Add(bankAccount);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = bankAccount.accountNo }, bankAccount);
        }

        // DELETE: api/BankAccounts/5
        [ResponseType(typeof(BankAccount))]
        public IHttpActionResult DeleteBankAccount(int id)
        {
            BankAccount bankAccount = db.BankAccount.Find(id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            db.BankAccount.Remove(bankAccount);
            db.SaveChanges();

            return Ok(bankAccount);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BankAccountExists(int id)
        {
            return db.BankAccount.Count(e => e.accountNo == id) > 0;
        }
    }
}