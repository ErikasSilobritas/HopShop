CREATE TABLE items (
item_id serial primary key,
item_name varchar(255) UNIQUE,
item_price decimal,
item_quantity int,
created_at timestamp default current_timestamp,
created_by varchar(255),
modified_at timestamp,
modified_by varchar(255),
is_deleted boolean default FALSE
);