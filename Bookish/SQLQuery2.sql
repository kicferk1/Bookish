select user_name, title 
from Books, Users, Borrows
where Borrows.user_id = Users.id and Borrows.book_id = Books.id