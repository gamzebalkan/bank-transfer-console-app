CREATE EXTENSION IF NOT EXISTS pgcrypto;

CREATE TABLE public.customers (
    customer_no TEXT PRIMARY KEY,
    full_name TEXT NOT NULL,
    balance NUMERIC NOT NULL,
    password TEXT,
    password_hash TEXT,
    currency TEXT
);

INSERT INTO public.customers (customer_no, full_name, balance, password, currency)
VALUES
('712201', 'Sophia Garcia',      18500.00, '7777', 'USD'),
('712202', 'John Miller',        2450.75,  '1234', 'USD'),
('712203', 'Ava Harris',         16200.00, '1010', 'USD'),
('712204', 'Michael Johnson',    820.00,   '2345', 'USD'),
('712205', 'Evelyn King',        22000.00, '5050', 'USD'),
('712206', 'David Smith',        15400.20, '3456', 'USD'),
('712207', 'Isabella Rodriguez', 14500.00, '8888', 'USD'),
('712208', 'James Brown',        560.10,   '4567', 'USD'),
('712209', 'Emily Clark',        19800.00, '5555', 'USD'),
('712210', 'Robert Williams',    9800.00,  '5678', 'USD'),
('712211', 'Olivia Martinez',    17500.50, '6666', 'USD'),
('712212', 'William Davis',      120.50,   '6789', 'USD'),
('712213', 'Amelia Walker',      21000.75, '3030', 'USD'),
('712214', 'Daniel Wilson',      4300.00,  '1111', 'USD'),
('712215', 'Mia Lee',            12800.90, '9999', 'USD'),
('712216', 'Matthew Taylor',     305.75,   '2222', 'USD'),
('712217', 'Charlotte Lewis',    15800.00, '2020', 'USD'),
('712218', 'Joseph Anderson',    15000.00, '3333', 'USD'),
('712219', 'Harper Young',       14200.10, '4040', 'USD'),
('712220', 'Christopher Thomas', 670.00,   '4444', 'USD');


UPDATE public.customers
SET password_hash = crypt(password, gen_salt('bf'));

ALTER TABLE public.customers
DROP COLUMN password;