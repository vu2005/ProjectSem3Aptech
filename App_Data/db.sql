create table claim (
    claim_id int primary key auto_increment,
    insurances_info_id int,
    claim_number varchar(50) not null,
    accident_date datetime not null,
    place_of_accident varchar(255),
    insured_amount decimal(10,2) not null,
    claimable_amount decimal(10,2) not null,
    created_at datetime default current_timestamp,
    foreign key (insurances_info_id) references insurance_info(insurance_info_id)
);

create table comment (
    comment_id int primary key auto_increment,
    customer_id int,
    parent_comment_id int,
    comment_text text not null,
    rating int,
    created_at datetime default current_timestamp,
    status varchar(50) not null,
    foreign key (customer_id) references customer(customer_id),
    foreign key (parent_comment_id) references comment(comment_id)
);

create table contact (
    id int primary key auto_increment,
    customer_id int,
    full_name varchar(255) not null,
    email varchar(255) not null,
    phone varchar(20) not null,
    subject varchar(255) not null,
    message text not null,
    date_added datetime default current_timestamp,
    date_modified datetime default current_timestamp on update current_timestamp,
    status bool not null,
    foreign key (customer_id) references customer(customer_id)
);

create table customer (
    customer_id int primary key auto_increment,
    user_id int,
    full_name varchar(100) not null,
    phone_number varchar(15) not null,
    address varchar(255) not null,
    foreign key (user_id) references user(user_id)
);

create table customer_support_request (
    support_id int primary key auto_increment,
    customer_id int,
    support_type varchar(100) not null,
    support_description text not null,
    support_payment text not null,
    support_status varchar(50) not null,
    created_at datetime default current_timestamp,
    resolved_at datetime,
    resolved_by int,
    foreign key (customer_id) references customer(customer_id),
    foreign key (resolved_by) references user(user_id)
);

create table insurance_info (
    insurance_info_id int primary key auto_increment,
    customer_id int,
    username varchar(255),
    email varchar(255),
    phone varchar(255),
    car_brand varchar(255),
    vehicle_line varchar(255),
    year_of_manufacture varchar(255),
    registration_date datetime,
    number_plate varchar(255),
    frame_number varchar(255),
    machine_number varchar(255),
    insurance_start_date datetime,
    insurance_package varchar(255),
    insurance_price decimal(10,2),
    created_at datetime default current_timestamp,
    foreign key (customer_id) references customer(customer_id)
);

create table insurance_service (
    service_id int primary key auto_increment,
    service_name varchar(100) not null,
    description text not null,
    price decimal(10,2) not null,
    image_url varchar(255),
    created_at datetime default current_timestamp,
    updated_at datetime,
    is_active bool default true
);

create table notification (
    notification_id int primary key auto_increment,
    customer_id int,
    message_type varchar(100) not null,
    message_content text not null,
    sent_at datetime default current_timestamp,
    is_read bool default false,
    foreign key (customer_id) references customer(customer_id)
);

create table payment (
    payment_id int primary key auto_increment,
    customer_id int,
    insurance_info_id int,
    bill_number varchar(50) not null,
    payment_date datetime,
    payment_amount decimal(10,2),
    transaction_id varchar(100),
    payment_status varchar(50) not null,
    foreign key (customer_id) references customer(customer_id),
    foreign key (insurance_info_id) references insurance_info(insurance_info_id)
);

create table user (
    user_id int primary key auto_increment,
    username varchar(50) not null,
    password varchar(255) not null,
    full_name varchar(100),
    email varchar(100) not null,
    phone_number varchar(15),
    address varchar(255),
    user_logs text not null,
    avatar varchar(255),
    role varchar(50) not null,
    created_at datetime default current_timestamp
);
CREATE TABLE history (
    history_id INT AUTO_INCREMENT PRIMARY KEY,
    customer_id INT NULL,
    username VARCHAR(255) NULL,
    email VARCHAR(255) NULL,
    phone VARCHAR(20) NULL,
    car_brand VARCHAR(255) NULL,
    vehicle_line VARCHAR(255) NULL,
    year_of_manufacture VARCHAR(4) NULL,
    registration_date DATETIME NULL,
    number_plate VARCHAR(50) NULL,
    frame_number VARCHAR(50) NULL,
    machine_number VARCHAR(50) NULL,
    insurance_start_date DATETIME NULL,
    insurance_package VARCHAR(255) NULL,
    insurance_code VARCHAR(100) NULL,
    insurance_price DECIMAL(18,2) NULL,
    insurance_end_date DATETIME NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (customer_id) REFERENCES Customer(customer_id)
);
