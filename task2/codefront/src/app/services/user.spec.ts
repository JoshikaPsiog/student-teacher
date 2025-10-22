import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { UserService } from './user.service';
import { AuthService } from './auth.service';

describe('UserService', () => {
  let service: UserService;
  let httpMock: HttpTestingController;
  let authService: jasmine.SpyObj<AuthService>;

  beforeEach(() => {
    const authServiceSpy = jasmine.createSpyObj('AuthService', ['getToken']);
    
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        UserService,
        { provide: AuthService, useValue: authServiceSpy }
      ]
    });
    
    service = TestBed.inject(UserService);
    httpMock = TestBed.inject(HttpTestingController);
    authService = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    
    authService.getToken.and.returnValue('fake-token');
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get all users', (done) => {
    const mockUsers = [
      { userId: 1, name: 'User 1', email: 'user1@test.com', designation: 'Student', dateOfBirth: '2000-01-01', isRegistered: true }
    ];

    service.getAllUsers().subscribe(users => {
      expect(users.length).toBe(1);
      done();
    });

    const req = httpMock.expectOne('http://localhost:5244/api/Users');
    expect(req.request.method).toBe('GET');
    req.flush(mockUsers);
  });
});
